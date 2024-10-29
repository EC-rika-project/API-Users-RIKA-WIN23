

namespace API_Users_RIKA_WIN23.Infrastructure.Entities;


public class UserWishListEntity
{
    public string UserID { get; set; } = null!;​

    public IEnumerable<string> ProductIDs { get; set; } = [];​
}
