namespace Fgs.Kernel.Specifications;

public interface ISpecification<T>
{
    bool IsSatisfiedBy(T candidate);
}
