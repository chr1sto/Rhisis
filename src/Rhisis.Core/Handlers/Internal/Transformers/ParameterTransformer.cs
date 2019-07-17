using System.Reflection;

namespace Rhisis.Core.Handlers.Internal.Transformers
{
    internal class ParameterTransformer : IParameterTransformer
    {
        private readonly ParameterTransformerCache _transformerCache;

        /// <summary>
        /// Creates a new <see cref="ParameterTransformer"/> instance.
        /// </summary>
        /// <param name="transformerCache">Parameter transformer cache.</param>
        public ParameterTransformer(ParameterTransformerCache transformerCache)
        {
            this._transformerCache = transformerCache;
        }

        /// <inheritdoc />
        public object Transform(object originalParameter, TypeInfo destinationParameterType)
        {
            ParameterTransformerCacheEntry transformer = this._transformerCache.GetTransformer(destinationParameterType);

            if (transformer == null)
                return null;

            object destinationParameter = transformer.ParameterFactory(destinationParameterType);

            return transformer.Transformer(originalParameter, destinationParameter);
        }
    }
}
