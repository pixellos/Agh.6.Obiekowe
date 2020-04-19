using System.Reflection;
using NJsonSchema.CodeGeneration.TypeScript;

namespace SigSpec.CodeGeneration.TypeScript
{
    public class SigSpecToTypeScriptGeneratorSettings : SigSpecToTypeScriptGeneratorSettingsBase
    {
        public SigSpecToTypeScriptGeneratorSettings()
            : base(new TypeScriptGeneratorSettings())
        {
            this.TypeScriptGeneratorSettings.TypeStyle = TypeScriptTypeStyle.Interface;
            this.CodeGeneratorSettings.TemplateFactory = new DefaultTemplateFactory(this.TypeScriptGeneratorSettings, new[]
            {
                typeof(TypeScriptGeneratorSettings).GetTypeInfo().Assembly,
                typeof(SigSpecToTypeScriptGeneratorSettingsBase).GetTypeInfo().Assembly,
            });
        }

        public TypeScriptGeneratorSettings TypeScriptGeneratorSettings => (TypeScriptGeneratorSettings)this.CodeGeneratorSettings;
    }
}
