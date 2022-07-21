namespace SquadronApi.Entities;

public class UploadedFile : BaseEntity
{
    public string Name { get; set; }
    public List<UploadedFileLine> Lines { get; set; }
}