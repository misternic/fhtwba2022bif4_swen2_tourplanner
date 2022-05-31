using System.Data;
using Npgsql;
using TourPlanner.Common;
using TourPlanner.Common.DTO;

namespace TourPlanner.DAL.Repositories;

public sealed class TourLogRepository : BaseRepository<TourLogDto>
{
    public TourLogRepository(DbContext context) : base(context) {}

    private const string FIELDS = "id, tour_id, date, difficulty, duration, rating, comment";

    private TourLogDto ReadAsTourLog(NpgsqlDataReader reader)
    {
        return new()
        {
            Id = reader.GetGuid(0),
            TourId = reader.GetGuid(1),
            Date = DateOnly.FromDateTime(reader.GetDateTime(2)),
            Difficulty = (Difficulty) reader.GetInt32(3),
            Duration = new TimeSpan(0, 0, reader.GetInt32(4)),
            Rating = reader.GetInt32(5),
            Comment = reader.GetString(6),
        };
    }
    
    public override TourLogDto GetById(Guid id)
    {
        var cmd = new NpgsqlCommand($"SELECT {FIELDS} FROM logs WHERE id=@id", Context.Connection, Context.Transaction);
        cmd.Parameters.AddWithValue("id", id);

        var reader = cmd.ExecuteReader(CommandBehavior.SingleResult);
        reader.Read();
        
        var log = ReadAsTourLog(reader);
        
        reader.Close();
        return log;
    }

    public override IEnumerable<TourLogDto> Get()
    {
        var cmd = new NpgsqlCommand($"SELECT {FIELDS} FROM logs", Context.Connection, Context.Transaction);
        var logs = new List<TourLogDto>();
        
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            logs.Add(ReadAsTourLog(reader));
        }
        reader.Close();

        return logs;
    }

    public override bool Insert(TourLogDto logDto)
    {
        var cmd = new NpgsqlCommand("INSERT INTO logs (id, tour_id, date, difficulty, duration, rating, comment) VALUES(@id, @tour_id, @date, @difficulty, @duration, @rating, @comment)", Context.Connection, Context.Transaction);
        cmd.Parameters.AddWithValue("id", logDto.Id);
        cmd.Parameters.AddWithValue("tour_id", logDto.TourId);
        cmd.Parameters.AddWithValue("date", logDto.Date);
        cmd.Parameters.AddWithValue("difficulty", (int) logDto.Difficulty);
        cmd.Parameters.AddWithValue("duration", logDto.Duration.TotalSeconds);
        cmd.Parameters.AddWithValue("rating", logDto.Rating);
        cmd.Parameters.AddWithValue("comment", logDto.Comment);

        if (cmd.ExecuteNonQuery() == 1)
        {
            Context.Commit();
            return true;
        }
        
        Context.Rollback();
        return false;
    }
    
    public override bool Update(TourLogDto logDto)
    {
        var cmd = new NpgsqlCommand("UPDATE logs SET date=@date, difficulty=@difficulty, duration=@duration, rating=@rating, comment=@comment WHERE id=@id", Context.Connection, Context.Transaction);
        cmd.Parameters.AddWithValue("id", logDto.Id);
        cmd.Parameters.AddWithValue("date", logDto.Date);
        cmd.Parameters.AddWithValue("difficulty", (int) logDto.Difficulty);
        cmd.Parameters.AddWithValue("duration", logDto.Duration.TotalSeconds);
        cmd.Parameters.AddWithValue("rating", logDto.Rating);
        cmd.Parameters.AddWithValue("comment", logDto.Comment);

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
        var cmd = new NpgsqlCommand("DELETE FROM logs WHERE id=@id", Context.Connection, Context.Transaction);
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