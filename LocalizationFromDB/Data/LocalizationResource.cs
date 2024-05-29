using System;
using System.Collections.Generic;

namespace LocalizationFromDB.Data;

public partial class LocalizationResource
{
    public int Id { get; set; }

    public string ResourceKey { get; set; } = null!;

    public string Value { get; set; } = null!;

    public string Culture { get; set; } = null!;
}
