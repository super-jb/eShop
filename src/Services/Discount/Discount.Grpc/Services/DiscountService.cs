using Ardalis.GuardClauses;
using AutoMapper;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IDiscountRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<DiscountService> _logger;

    public DiscountService(IDiscountRepository repository, IMapper mapper, ILogger<DiscountService> logger)
    {
        _repository = Guard.Against.Null(repository, nameof(repository));
        _mapper = Guard.Against.Null(mapper, nameof(mapper));
        _logger = Guard.Against.Null(logger, nameof(logger));
    }

    public override async Task<Protos.CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        Entities.Coupon coupon = await _repository.GetDiscount(request.ProductName);
        if (coupon == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
        }
        _logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

        return _mapper.Map<Protos.CouponModel>(coupon);
    }

    public override async Task<Protos.CouponModel> CreateDiscount(Protos.CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Entities.Coupon>(request.Coupon);

        await _repository.CreateDiscount(coupon);
        _logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", coupon.ProductName);

        return _mapper.Map<Protos.CouponModel>(coupon);
    }

    public override async Task<Protos.CouponModel> UpdateDiscount(Protos.UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Entities.Coupon>(request.Coupon);

        await _repository.UpdateDiscount(coupon);
        _logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", coupon.ProductName);

        var couponModel = _mapper.Map<Protos.CouponModel>(coupon);
        return couponModel;
    }

    public override async Task<Protos.DeleteDiscountResponse> DeleteDiscount(Protos.DeleteDiscountRequest request, ServerCallContext context)
    {
        var deleted = await _repository.DeleteDiscount(request.ProductName);
        var response = new Protos.DeleteDiscountResponse
        {
            Success = deleted
        };

        return response;
    }
}
