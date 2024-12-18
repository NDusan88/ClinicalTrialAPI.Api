using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.IO;

namespace ClinicalTrialAPI.Infrastructure.Services
{
    public interface IJsonSchemaValidator
    {
        bool Validate(string jsonContent, out IList<string> errors);
    }

    public class JsonSchemaValidator : IJsonSchemaValidator
    {
        private readonly JSchema _schema;

        public JsonSchemaValidator()
        {
            // Load the schema from the JSON schema file
            var schemaFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Schemas", "ClinicalTrialSchema.json");

            if (!File.Exists(schemaFilePath))
                throw new FileNotFoundException("JSON Schema file not found.", schemaFilePath);

            var schemaJson = File.ReadAllText(schemaFilePath);
            _schema = JSchema.Parse(schemaJson);
        }

        public bool Validate(string jsonContent, out IList<string> errors)
        {
            errors = new List<string>();

            try
            {
                // Parse the input JSON content
                var json = JObject.Parse(jsonContent);

                // Validate against the schema
                if (!json.IsValid(_schema, out errors))
                {
                    return false; // Validation failed
                }

                return true; // Validation succeeded
            }
            catch (JsonReaderException ex)
            {
                errors.Add($"Invalid JSON format: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                errors.Add($"Validation error: {ex.Message}");
                return false;
            }
        }
    }
}
