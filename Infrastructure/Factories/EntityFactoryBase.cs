using System.Linq.Expressions;
using Core.Interfaces.Factories;
using Infrastructure.Helper;

namespace Infrastructure.Factories;

/// <summary>
/// Central conversion layer between Domain Models and Entity Framework Core entities
/// Originally prototyped with Phind AI (preserved for historical reference)
/// Key Responsibilities:
/// 1. Bidirectional model conversion with property reflection
/// 2. Expression tree translation for cross-layer queries
/// three. Foundation for type-safe repository implementations
/// </summary>
/// <typeparam name="TDomain">Domain Model (Core/Domain Layer)</typeparam>
/// <typeparam name="TEntity">Database Entity (Infrastructure Layer)</typeparam>
/// <remarks>
/// Implements Clean Architecture principles:
/// - Core/Domain layers never reference EF entities
/// - Infrastructure depends on Core contracts
/// - Reflection-based mapping with fallback to explicit implementations
/// </remarks>
/// <example>
/// <code>
/// // Auto-mapped conversion
/// var factory = new EntityFactoryBase&lt;Status, StatusEntity;
/// var domainEntity = factory.ToDomain(dbEntity);
///
/// // Customized conversion
/// public class AuditFactory: EntityFactoryBase&lt;Audit, AuditEntity&gt;
/// {
///     public override Audit ToDomain(AuditEntity entity) => new()
///     {
///         ID = entity. ID,
///         Changes = JsonConvert.DeserializeObject(entity.ChangeJson)
///     };
/// }
/// </code>
/// </example>
public class EntityFactoryBase<TDomain, TEntity> : IEntityFactory<TDomain, TEntity>
    where TDomain : class
    where TEntity : class
{
    /// <summary>
    /// Converts Entity Framework entity to a domain model using reflection
    /// </summary>
    /// <param name="entity">Database entity instance</param>
    /// <returns>Domain model with copied properties</returns>
    /// <remarks>
    /// Uses <see cref="PropertyReflectionHelper"/> for automatic property matching
    /// Override for custom mapping logic when property names differ
    /// </remarks>
    public virtual TDomain ToDomain(TEntity entity)
    {
        return PropertyReflectionHelper.CopyProperties<TDomain, TEntity>(entity);
    }

    /// <summary>
    /// Converts a domain model to Entity Framework entity using reflection
    /// </summary>
    /// <param name="domain">Domain model instance</param>
    /// <returns>Database entity with copied properties</returns>
    /// <remarks>
    /// Preserves layer isolation - Core never contains entity references
    /// Automatic null handling for nested complex types
    /// </remarks>
    public virtual TEntity ToEntity(TDomain domain)
    {
        return PropertyReflectionHelper.CopyProperties<TEntity, TDomain>(domain);
    }

    /// <summary>
    /// This is 100% Generated by AI Phind
    /// Translates domain-level LINQ predicates to entity-compatible expressions
    /// </summary>
    /// <param name="domainPredicate">Domain model query expression</param>
    /// <returns>Entity-compatible query expression</returns>
    /// <remarks>
    /// Enables type-safe queries across architectural layers
    /// Uses expression tree visitor pattern for parameter type substitution
    /// </remarks>
    public Expression<Func<TEntity, bool>> CreateEntityPredicate(
        Expression<Func<TDomain, bool>>? domainPredicate
    )
    {
        // Create a new parameter for the entity type
        // Now it handles nullable predicates, gives us
        // return all when no filter is provided
        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        // If the domain predicate is null, return null
        if (domainPredicate == null)
            return e => true;
        var visitor = new ParameterTypeVisitor<TDomain, TEntity>(
            domainPredicate.Parameters[0],
            parameter
        );
        var body = visitor.Visit(domainPredicate.Body);
        return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
    }

    /// <summary>
    /// This is 90% Generated by AI Phind
    /// Inspired by https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression.parameter?view=net-9.0
    /// </summary>
    /// <param name="domainInclude"></param>
    /// <returns></returns>
    public Expression<Func<TEntity, object>> CreateEntityInclude(
        Expression<Func<TDomain?, object>> domainInclude
    )
    {
        // Create a new parameter for the entity type
        var param = Expression.Parameter(typeof(TEntity), "entity");
        // Create a new visitor to replace the parameter type
        var visitor = new ParameterTypeVisitor<TDomain, TEntity>(
            domainInclude.Parameters[0],
            param
        );
        // Visit the body of the domain include expression
        var body = visitor.Visit(domainInclude.Body);
        // Return the new expression with the entity parameter
        return Expression.Lambda<Func<TEntity, object>>(body, param);
    }
}

/// <summary>
/// This is 100% Generated by an AI Phind
/// Expression tree visitor that replaces parameter types in LINQ predicates
/// Enables type-safe query translation between domain models and EF entities
/// </summary>
/// <typeparam name="TFrom">Source type (Domain Model)</typeparam>
/// <typeparam name="TTo">Target type (EF Entity)</typeparam>
/// <remarks>
/// Key Features:
/// 1. Essential for maintaining layer separation in predicate conversion
/// 2. Enables LINQ expressions using domain models to execute against EF entities
/// 3. Works with any property names that match between domain and entity types
/// </remarks>
internal class ParameterTypeVisitor<TFrom, TTo>(
    ParameterExpression oldParameter,
    ParameterExpression newParameter
) : ExpressionVisitor
{
    /// <summary>
    /// This was 100% Generated by AI Phind
    /// Replaces expression parameters during tree traversal
    /// </summary>
    /// <param name="node">Current expression node</param>
    /// <returns>Modified expression with an updated parameter type</returns>
    /// <remarks>
    /// Core mechanism enabling type-safe cross-layer queries:
    /// - Swaps domain model parameters with entity parameters
    /// - Preserves expression logic while changing execution target
    /// - Critical for repository pattern implementation
    /// </remarks>
    protected override Expression VisitMember(MemberExpression node)
    {
        // If the expression is not a member, return the base visit
        if (node.Expression != oldParameter)
            return base.VisitMember(node);

        // Get the property name
        var properTyName = node.Member.Name;
        // Create a new property with the same name on the new parameter with suffix Entity
        var newProperty =
            typeof(TTo).GetProperty(properTyName)
            ?? typeof(TTo).GetProperty(properTyName + "Entity")
            ?? typeof(TTo).GetProperties().FirstOrDefault(p => p.Name.EndsWith(properTyName));

        // Return the new property if it exists
        return newProperty != null
            ? Expression.Property(newParameter, newProperty)
            : throw new InvalidOperationException(
                $"Property {properTyName} not found on {typeof(TTo).Name}"
            );
    }

    /// <summary>
    /// This was 100% Generated by AI Phind
    /// Replaces expression parameters during tree traversal
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    protected override Expression VisitParameter(ParameterExpression node)
    {
        // If the parameter is the old parameter, replace it with the new parameter
        return node == oldParameter ? newParameter : base.VisitParameter(node);
    }
}
