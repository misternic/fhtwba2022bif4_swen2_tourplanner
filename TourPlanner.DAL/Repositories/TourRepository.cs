using System.Data;
using Npgsql;
using TourPlanner.Common;

namespace TourPlanner.DAL.Repositories;

public sealed class TourRepository : BaseRepository<Tour>
{
    public TourRepository(DbContext context) : base(context)
    {
    }

    private const string Fields =
        "id, name, description, \"from\", \"to\", transport_type, distance, estimated_duration";

    private static Tour ReadAsTour(NpgsqlDataReader reader)
    {
        return new Tour
        {
            Id = reader.GetGuid(0),
            Name = reader.GetString(1),
            Description = reader.GetString(2),
            From = reader.GetString(3),
            To = reader.GetString(4),
            TransportType = (TransportType) reader.GetInt32(5),
            Distance = reader.GetDouble(6),
            EstimatedTime = new TimeSpan(0, 0, reader.GetInt32(7))
        };
    }

    public override Tour GetById(Guid id)
    {
        const string logsCount = "SELECT COUNT(*) FROM logs WHERE tour_id=@id";
        const string avgDifficulty = "SELECT AVG(difficulty) FROM logs WHERE tour_id=@id";
        const string avgDuration = "SELECT AVG(duration) FROM logs WHERE tour_id=@id";

        var cmd = new NpgsqlCommand(
            $"SELECT {Fields}, ({logsCount}), ({avgDifficulty}), ({avgDuration}) FROM tours WHERE id=@id",
            Context.Connection, Context.Transaction);
        cmd.Parameters.AddWithValue("id", id);

        var reader = cmd.ExecuteReader(CommandBehavior.SingleResult);
        reader.Read();

        var tour = ReadAsTour(reader);

        if (reader.GetInt32(8) > 0)
        {
            tour.Popularity = reader.GetInt32(8);
            tour.ChildFriendlyness = reader.GetDouble(10) / reader.GetDouble(9);
        }

        reader.Close();
        return tour;
    }

    public override IEnumerable<Tour> Get()
    {
        var cmd = new NpgsqlCommand($"SELECT {Fields} FROM tours", Context.Connection, Context.Transaction);
        var tours = new List<Tour>();

        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            tours.Add(ReadAsTour(reader));
        }

        reader.Close();

        return tours;
    }

    public override bool Insert(Tour tour)
    {
        var cmd = new NpgsqlCommand(
            "INSERT INTO tours (id, name, description, \"from\", \"to\", transport_type, distance, estimated_duration) VALUES(@id, @name, @description, @from, @to, @transport_type, @distance, @estimated_duration)",
            Context.Connection, Context.Transaction);
        cmd.Parameters.AddWithValue("id", tour.Id);
        cmd.Parameters.AddWithValue("name", tour.Name);
        cmd.Parameters.AddWithValue("description", tour.Description);
        cmd.Parameters.AddWithValue("from", tour.From);
        cmd.Parameters.AddWithValue("to", tour.To);
        cmd.Parameters.AddWithValue("transport_type", (int) tour.TransportType);
        cmd.Parameters.AddWithValue("distance", tour.Distance);
        cmd.Parameters.AddWithValue("estimated_duration", tour.EstimatedTime.TotalSeconds);

        if (cmd.ExecuteNonQuery() == 1)
        {
            Context.Commit();
            return true;
        }

        Context.Rollback();
        return false;
    }

    public override bool Update(Tour tour)
    {
        var cmd = new NpgsqlCommand(
            "UPDATE tours SET name=@name, description=@description, \"from\"=@from, \"to\"=@to, transport_type=@transport_type, distance=@distance, estimated_duration=@estimated_duration WHERE id=@id",
            Context.Connection, Context.Transaction);
        cmd.Parameters.AddWithValue("id", tour.Id);
        cmd.Parameters.AddWithValue("name", tour.Name);
        cmd.Parameters.AddWithValue("description", tour.Description);
        cmd.Parameters.AddWithValue("from", tour.From);
        cmd.Parameters.AddWithValue("to", tour.To);
        cmd.Parameters.AddWithValue("transport_type", (int) tour.TransportType);
        cmd.Parameters.AddWithValue("distance", tour.Distance);
        cmd.Parameters.AddWithValue("estimated_duration", tour.EstimatedTime.TotalSeconds);

        if (cmd.ExecuteNonQuery() == 1)
        {
            Context.Commit();
            return true;
        }

        Context.Rollback();
        return false;
    }

    public override bool Delete(Guid id)
    {
        var cmd = new NpgsqlCommand("DELETE FROM tours WHERE id=@id", Context.Connection, Context.Transaction);
        cmd.Parameters.AddWithValue("id", id);

        if (cmd.ExecuteNonQuery() == 1)
        {
            Context.Commit();
            return true;
        }

        Context.Rollback();
        return false;
    }
}