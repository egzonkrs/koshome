using System.Collections.Generic;

namespace KosHome.Domain.Entities.Users;

public class IdentityRole
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IDictionary<string, IEnumerable<string>> Attributes { get; set; }
}
