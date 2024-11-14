namespace TemplateSolution.Application.Services;

/// <summary>
/// business servislerimiz için marker interface'i. 
/// Bu interface'den türeyen tüm servisler otomatik olarak DI katmanına eklenir.
/// Ör: public class ProductService : IProductService, IBusinessService 
/// </summary>
public interface IBusinessService
{
}
