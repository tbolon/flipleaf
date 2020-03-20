using System;
using System.Threading.Tasks;

using FlipLeaf.Core.Text;

namespace FlipLeaf.Core.Pipelines
{
    public class TextTransformPipeline : IPipeline
    {
        private readonly Func<IInput, bool> _filter;
        private readonly ITextMiddleware _readMiddleware;
        private readonly ITextMiddleware[] _prepareMiddlewares;
        private readonly ITextMiddleware[] _transformMiddlewares;
        private readonly ITextContentWriter _writer;

        public TextTransformPipeline(Func<IInput, bool> filter,
            ITextMiddleware readMiddleware,
            ITextMiddleware[] prepareMiddlewares,
            ITextMiddleware[] transformMiddlewares,
            ITextContentWriter writer)
        {
            _filter = filter;
            _readMiddleware = readMiddleware;
            _prepareMiddlewares = prepareMiddlewares;
            _transformMiddlewares = transformMiddlewares;
            _writer = writer;
        }

        public bool Accept(IWebSite site, IInput input)
            => _filter(input);

        public Task<InputItems> PrepareAsync(IWebSite site, IInput input)
            => ExecuteAsync(site, input, _prepareMiddlewares, false);

        public Task TransformAsync(IWebSite site, IInput input)
            => ExecuteAsync(site, input, _transformMiddlewares, true);

        private async Task<InputItems> ExecuteAsync(IWebSite site, IInput input, ITextMiddleware[] middlewares, bool write)
        {
            try
            {
                var textContext = new TextInputContext(site, input);

                var executor = new InputTransformer(_readMiddleware, middlewares, write ? _writer : null, textContext);
                await executor.ExecuteAsync().ConfigureAwait(false);

                return textContext.Items;
            }
            catch (Exception e)
            {
                throw new ParseException($"Error while parsing file {input.RelativeName}", e);
            }
        }

        private class InputTransformer
        {
            private readonly ITextMiddleware _readMiddleware;
            private readonly ITextMiddleware[] _middlewares;
            private readonly ITextContentWriter _writer;
            private readonly TextInputContext _ctx;
            private int _index;

            public InputTransformer(
                ITextMiddleware readMiddleware,
                ITextMiddleware[] middlewares,
                ITextContentWriter writer,
                TextInputContext ctx)
            {
                _readMiddleware = readMiddleware;
                _middlewares = middlewares;
                _writer = writer;
                _ctx = ctx;
            }

            public async Task ExecuteAsync()
            {
                if (_readMiddleware != null)
                {
                    await _readMiddleware.InvokeAsync(_ctx, InvokeNext).ConfigureAwait(false);
                }
                else
                {
                    await InvokeNext().ConfigureAwait(false);
                }

                if (_writer != null)
                {
                    await _writer.InvokeAsync(_ctx);
                }
            }

            private async Task InvokeNext()
            {
                var mw = GetNextMiddleware();
                if (mw == null)
                {
                    return;
                }

                await mw.InvokeAsync(_ctx, InvokeNext).ConfigureAwait(false);
            }

            private Task InvokeWriter() => _writer.InvokeAsync(_ctx);

            private ITextMiddleware GetNextMiddleware()
            {
                if (_index >= _middlewares.Length)
                {
                    return null;
                }

                var mw = _middlewares[_index];
                _index++;
                return mw;
            }
        }
    }
}
