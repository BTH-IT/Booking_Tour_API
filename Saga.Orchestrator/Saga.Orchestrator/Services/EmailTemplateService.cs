namespace Saga.Orchestrator.Services
{
    public class EmailTemplateService
    {
        private static readonly string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string _templateFolder = Path.Combine(_baseDirectory, "EmailTemplates");

        protected string ReadEmailTemplateContent(string templateEmailName,string format = "html")
        {
            var filePath = Path.Combine(_templateFolder, templateEmailName + "." +format);

            using var fs = new FileStream(filePath, FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
            using var sr = new StreamReader(fs);

            var text= sr.ReadToEnd();
            sr.Close();

            return text;

        }
    }
}
