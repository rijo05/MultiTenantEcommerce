﻿using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Templates.Entities;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class EmailTemplateRepository : Repository<EmailTemplate>, IEmailTemplateRepository
{
    public EmailTemplateRepository(AppDbContext appDbContext, ITenantContext tenantContext)
        : base(appDbContext, tenantContext)
    {
    }

    public async Task<EmailTemplate?> GetDefaultTemplateByTemplateName(EmailTemplateNames templateName)
    {
        return await _appDbContext.EmailTemplates.IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.TemplateName == templateName.ToString() && x.IsActive == true);
    }
}
