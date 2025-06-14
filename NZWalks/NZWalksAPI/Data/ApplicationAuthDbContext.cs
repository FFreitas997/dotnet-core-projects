using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalksAPI.Data;

public class ApplicationAuthDbContext(DbContextOptions<ApplicationAuthDbContext> options) : IdentityDbContext(options)
{
}