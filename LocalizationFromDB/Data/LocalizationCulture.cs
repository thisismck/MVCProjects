using System;
using System.Collections.Generic;

namespace LocalizationFromDB.Data;

public partial class LocalizationCulture
{
    public int Id { get; set; }

    public string CultureCode { get; set; } = null!;

    public string DisplayName { get; set; } = null!;
}
