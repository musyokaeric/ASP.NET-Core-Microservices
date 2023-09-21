using AutoMapper;
using Discount.GRPC.Entities;
using Discount.GRPC.Protos;
using Discount.GRPC.Repositories.Interface;
using Grpc.Core;

namespace Discount.GRPC.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository discountRepository;
        private readonly ILogger<DiscountService> logger;
        private readonly IMapper mapper;

        public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger, IMapper mapper)
        {
            this.discountRepository = discountRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await discountRepository.GetDiscount(request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
            }
            logger.LogInformation($"Discount is retrieved for ProductName: {coupon.ProductName}, Amount: {coupon.Amount}");
            return mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = await discountRepository.CreateDiscount(mapper.Map<Coupon>(request.Coupon));
            logger.LogInformation($"Discount created successfully. ProductName: {request.Coupon.ProductName}");
            return mapper.Map<CouponModel>(coupon);
        }

        public async override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = await discountRepository.UpdateDiscount(mapper.Map<Coupon>(request.Coupon));
            logger.LogInformation($"Discount created successfully. ProductName: {request.Coupon.ProductName}");
            return mapper.Map<CouponModel>(coupon);
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await discountRepository.DeleteDiscount(request.ProductName);
            return new DeleteDiscountResponse { Success = deleted };
        }
    }
}
