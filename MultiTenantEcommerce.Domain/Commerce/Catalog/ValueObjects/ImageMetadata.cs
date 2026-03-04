using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Domain.Commerce.Catalog.ValueObjects;
public record ImageMetadata(
    string FileName,
    long Size,
    string ContentType,
    bool IsMain);
