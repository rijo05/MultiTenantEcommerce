using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MultiTenantEcommerce.Domain.Templates.Entities;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using System.Text.RegularExpressions;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Seed;
public static class EmailTemplateSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider, string templatePath)
    {
        var scope = serviceProvider.CreateScope();
        var _appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var files = Directory.GetFiles(templatePath, "*.html");

        foreach (var item in files)
        {
            var templateName = Path.GetFileNameWithoutExtension(item);
            var html = await File.ReadAllTextAsync(item);

            var titleMatch = Regex.Match(html, @"<title>(.*?)</title>", RegexOptions.IgnoreCase);
            var subject = titleMatch.Success ? titleMatch.Groups[1].Value.Trim() : templateName;

            var existingTemplate = await _appDbContext.EmailTemplates.FirstOrDefaultAsync(x => x.TemplateName == templateName);

            if (existingTemplate is not null)
            {
                existingTemplate.HtmlContent = html;
                existingTemplate.Subject = subject;
                existingTemplate.SetUpdatedAt();

                _appDbContext.EmailTemplates.Update(existingTemplate);
            }
            else
            {
                var emailTemplate = new EmailTemplate(templateName, true, subject, html);

                await _appDbContext.EmailTemplates.AddAsync(emailTemplate);
            }
        }

        await _appDbContext.SaveChangesAsync();
    }
}
