using System;

namespace Telluria.Utils.Crud.Services;

public class TenantService : ITenantService
{
  public Guid TenantId { get; set; }

  public TenantService()
  {
  }

  public bool SetTenant(Guid tenant)
  {
    TenantId = tenant;

    return true;
  }
}
