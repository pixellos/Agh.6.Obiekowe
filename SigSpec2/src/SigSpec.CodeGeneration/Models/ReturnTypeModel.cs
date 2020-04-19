using NJsonSchema;
using NJsonSchema.CodeGeneration;

namespace SigSpec.CodeGeneration.Models
{
    public class ReturnTypeModel
    {
        private readonly JsonSchema _parameter;
        private readonly TypeResolverBase _resolver;

        public ReturnTypeModel(JsonSchema parameter, TypeResolverBase resolver)
        {
            this._parameter = parameter;
            this._resolver = resolver;

            this.Type = this._resolver.Resolve(this._parameter.ActualTypeSchema, this._parameter.IsNullable(SchemaType.JsonSchema), null);
        }

        public string Type { get; }
    }
}