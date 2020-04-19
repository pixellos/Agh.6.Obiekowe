using NJsonSchema.CodeGeneration;

namespace SigSpec.CodeGeneration
{
    public class SigSpecToTypeScriptGeneratorSettingsBase : ClientGeneratorBaseSettings
    {
        public SigSpecToTypeScriptGeneratorSettingsBase(CodeGeneratorSettingsBase codeGeneratorSettings)
        {
            this.CodeGeneratorSettings = codeGeneratorSettings;
        }

        protected CodeGeneratorSettingsBase CodeGeneratorSettings { get; }
    }
}
