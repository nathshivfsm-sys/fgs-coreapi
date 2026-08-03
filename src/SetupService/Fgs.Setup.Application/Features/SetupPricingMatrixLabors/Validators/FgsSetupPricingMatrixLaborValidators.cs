using Fgs.Setup.Application.Abstractions.SetupLaborRateTypes;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrices;
using Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Commands.CreateFgsSetupPricingMatrixLabor;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Commands.UpdateFgsSetupPricingMatrixLabor;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Commands.PatchFgsSetupPricingMatrixLabor;
using FluentValidation;

namespace Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Validators;

public sealed class CreateFgsSetupPricingMatrixLaborCommandValidator : AbstractValidator<CreateFgsSetupPricingMatrixLaborCommand>
{
    public CreateFgsSetupPricingMatrixLaborCommandValidator(IFgsSetupPricingMatrixReadRepository matrices, IFgsSetupLaborRateTypeReadRepository rateTypes, IFgsSetupTechSkillLevelReadRepository skills)
    {
        RuleFor(x=>x.Dto.PricingMatrixId).GreaterThan(0).MustAsync((id,ct)=>matrices.ExistsByIdAsync(id,ct)).WithMessage("The specified pricing matrix was not found.");
        RuleFor(x=>x.Dto.LaborRateTypeId).GreaterThan(0).MustAsync(async (id,ct)=>await rateTypes.GetByIdAsync(id,ct) is not null).WithMessage("The specified labor rate type was not found.");
        RuleFor(x=>x.Dto).CustomAsync(async (dto,context,ct)=>await ValidateAsync(dto.PricingMatrixId,dto.TechSkillLevelId,dto.BaseRate,dto.OvertimeMultiplier,dto.DoubleTimeMultiplier,dto.DiscountPercent,matrices,skills,context,ct));
    }
    internal static async Task ValidateAsync(long matrixId,long? skillId,decimal baseRate,decimal? ot,decimal? dt,decimal? discount,IFgsSetupPricingMatrixReadRepository matrices,IFgsSetupTechSkillLevelReadRepository skills,ValidationContext<CreateFgsSetupPricingMatrixLaborCommand> context,CancellationToken ct)
    {
        var flags=await matrices.GetFlagsByIdAsync(matrixId,ct); if(flags is null)return;
        if(!flags.IsLaborRateBySkillLevel && skillId.HasValue)context.AddFailure("Dto.TechSkillLevelId","Tech skill level must be null when labor rates are not by skill level.");
        if(flags.IsLaborRateBySkillLevel && !skillId.HasValue)context.AddFailure("Dto.TechSkillLevelId","Tech skill level is required.");
        if(skillId.HasValue && await skills.GetByIdAsync(skillId.Value,ct) is null)context.AddFailure("Dto.TechSkillLevelId","The specified tech skill level was not found.");
        if(!flags.IsLaborTierStructure && baseRate<0)context.AddFailure("Dto.BaseRate","Base rate must be greater than or equal to zero.");
        if(!flags.IsLaborTierStructure && ot.HasValue && ot<1)context.AddFailure("Dto.OvertimeMultiplier","Overtime multiplier must be at least 1.");
        if(!flags.IsLaborTierStructure && dt.HasValue && dt<1)context.AddFailure("Dto.DoubleTimeMultiplier","Double-time multiplier must be at least 1.");
        if(!flags.IsLaborTierStructure && discount.HasValue && (discount<0||discount>100))context.AddFailure("Dto.DiscountPercent","Discount percent must be between 0 and 100.");
    }
}
public sealed class UpdateFgsSetupPricingMatrixLaborCommandValidator : AbstractValidator<UpdateFgsSetupPricingMatrixLaborCommand>
{
    public UpdateFgsSetupPricingMatrixLaborCommandValidator(IFgsSetupPricingMatrixReadRepository matrices,IFgsSetupLaborRateTypeReadRepository rateTypes,IFgsSetupTechSkillLevelReadRepository skills)
    {
        RuleFor(x=>x.Id).GreaterThan(0); RuleFor(x=>x.Dto.PricingMatrixId).MustAsync((id,ct)=>matrices.ExistsByIdAsync(id,ct)).WithMessage("The specified pricing matrix was not found.");
        RuleFor(x=>x.Dto.LaborRateTypeId).GreaterThan(0).MustAsync(async(id,ct)=>await rateTypes.GetByIdAsync(id,ct)is not null).WithMessage("The specified labor rate type was not found.");
        RuleFor(x=>x.Dto).CustomAsync(async(dto,c,ct)=>{var flags=await matrices.GetFlagsByIdAsync(dto.PricingMatrixId,ct);if(flags is null)return;if(!flags.IsLaborRateBySkillLevel&&dto.TechSkillLevelId.HasValue)c.AddFailure("Dto.TechSkillLevelId","Tech skill level must be null.");if(flags.IsLaborRateBySkillLevel&&!dto.TechSkillLevelId.HasValue)c.AddFailure("Dto.TechSkillLevelId","Tech skill level is required.");if(dto.TechSkillLevelId.HasValue&&await skills.GetByIdAsync(dto.TechSkillLevelId.Value,ct)is null)c.AddFailure("Dto.TechSkillLevelId","The specified tech skill level was not found.");if(!flags.IsLaborTierStructure&&dto.BaseRate<0)c.AddFailure("Dto.BaseRate","Base rate must be nonnegative.");if(!flags.IsLaborTierStructure&&dto.OvertimeMultiplier is <1)c.AddFailure("Dto.OvertimeMultiplier","Overtime multiplier must be at least 1.");if(!flags.IsLaborTierStructure&&dto.DoubleTimeMultiplier is <1)c.AddFailure("Dto.DoubleTimeMultiplier","Double-time multiplier must be at least 1.");if(!flags.IsLaborTierStructure&&dto.DiscountPercent is <0 or >100)c.AddFailure("Dto.DiscountPercent","Discount percent must be between 0 and 100.");});
    }
}
public sealed class PatchFgsSetupPricingMatrixLaborCommandValidator : AbstractValidator<PatchFgsSetupPricingMatrixLaborCommand>
{
    public PatchFgsSetupPricingMatrixLaborCommandValidator(){RuleFor(x=>x.Id).GreaterThan(0);RuleFor(x=>x.Dto.BaseRate).GreaterThanOrEqualTo(0).When(x=>x.Dto.BaseRate.HasValue);RuleFor(x=>x.Dto.OvertimeMultiplier).GreaterThanOrEqualTo(1).When(x=>x.Dto.OvertimeMultiplier.HasValue);RuleFor(x=>x.Dto.DoubleTimeMultiplier).GreaterThanOrEqualTo(1).When(x=>x.Dto.DoubleTimeMultiplier.HasValue);RuleFor(x=>x.Dto.DiscountPercent).InclusiveBetween(0,100).When(x=>x.Dto.DiscountPercent.HasValue);}
}
