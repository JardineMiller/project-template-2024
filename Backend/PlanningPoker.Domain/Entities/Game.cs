using PlanningPoker.Domain.Entities.Base;

#pragma warning disable CS8618

namespace PlanningPoker.Domain.Entities;

public class Game : DeletableEntity
{
    public string Name { get; init; }
    public string? Description { get; init; }
    public string Code { get; init; }

    public string OwnerId { get; init; }
    public User? Owner { get; init; }

    // TODO: List of  stories
    // - name
    // - description
    // - list of votes
    // - user id
    // - name
    // - vote
    // - link
}
