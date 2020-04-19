using NJsonSchema;
using NJsonSchema.Generation;
using System;
using System.Linq;

namespace SigSpec.Core
{
    public class SigSpecSchemaResolver : JsonSchemaResolver
    {
        private readonly SigSpecDocument _document;
        private readonly JsonSchemaGeneratorSettings _settings;

        public SigSpecSchemaResolver(SigSpecDocument document, SigSpecGeneratorSettings settings) 
            : base(document, settings)
        {
            this._document = document;
            this._settings = settings;
        }

        public override void AppendSchema(JsonSchema schema, string typeNameHint)
        {
            // TODO: JsonSchemaResolver should use new IDefinitionsObject interface and not JsonSchema4

            if (schema == null)
                throw new ArgumentNullException(nameof(schema));
            if (schema == this.RootObject)
                throw new ArgumentException("The root schema cannot be appended.");

            if (!this._document.Definitions.Values.Contains(schema))
            {
                var typeName = this._settings.TypeNameGenerator.Generate(schema, typeNameHint, this._document.Definitions.Keys);
                if (!string.IsNullOrEmpty(typeName) && !this._document.Definitions.ContainsKey(typeName))
                    this._document.Definitions[typeName] = schema;
                else
                    this._document.Definitions["ref_" + Guid.NewGuid().ToString().Replace("-", "_")] = schema;
            }
        }
    }
}
