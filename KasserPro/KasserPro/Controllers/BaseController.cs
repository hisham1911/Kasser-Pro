using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KasserPro.Api.Controllers
{
    /// <summary>
    /// Base controller with common multi-tenant helpers
    /// </summary>
    public abstract class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Get the current user's StoreId from JWT claims
        /// </summary>
        protected int GetStoreId()
        {
            var storeIdClaim = User.FindFirst("StoreId");
            if (storeIdClaim == null)
            {
                throw new UnauthorizedAccessException("StoreId not found in token");
            }
            return int.Parse(storeIdClaim.Value);
        }

        /// <summary>
        /// Get the current user's ID from JWT claims
        /// </summary>
        protected int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("UserId not found in token");
            }
            return int.Parse(userIdClaim.Value);
        }

        /// <summary>
        /// Get the current user's role from JWT claims
        /// </summary>
        protected string GetUserRole()
        {
            var roleClaim = User.FindFirst(ClaimTypes.Role);
            return roleClaim?.Value ?? string.Empty;
        }

        /// <summary>
        /// Check if current user is Owner or Manager
        /// </summary>
        protected bool IsManagerOrOwner()
        {
            var role = GetUserRole();
            return role == "Owner" || role == "Manager" || role == "SuperAdmin";
        }

        /// <summary>
        /// Check if current user is SuperAdmin
        /// </summary>
        protected bool IsSuperAdmin()
        {
            return GetUserRole() == "SuperAdmin";
        }
    }
}
