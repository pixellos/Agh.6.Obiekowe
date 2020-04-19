namespace SigSpec.CodeGeneration
{
    public class ClientGeneratorBaseSettings
    {
        public ClientGeneratorBaseSettings()
        {
            this.GenerateDtoTypes = true;
        }

        /// <summary>Gets or sets a value indicating whether to generate DTO classes (default: true).</summary>
        public bool GenerateDtoTypes { get; set; }
    }
}