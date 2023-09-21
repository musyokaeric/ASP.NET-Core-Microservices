using Discount.GRPC.Protos;

namespace Discount.GRPC.GrpcServices
{
    public class DisccountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient discountProtoService;

        public DisccountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        {
            this.discountProtoService = discountProtoService;
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discount = new GetDiscountRequest { ProductName = productName };
            return await discountProtoService.GetDiscountAsync(discount);
        }
    }
}
