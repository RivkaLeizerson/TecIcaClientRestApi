using Markdig;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TecIcaClient.WebApi.Pages;

public class ReadmeModel : PageModel
{
	public string HtmlContent { get; private set; }

	public void OnGet()
	{
		var currentDirectory = Environment.CurrentDirectory;

		var markdownContent = System.IO.File.ReadAllText(@"Documentation\README.md");
		HtmlContent = Markdown.ToHtml(markdownContent);
	}
}