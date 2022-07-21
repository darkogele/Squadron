namespace SquadronApi.Entities;

public class UploadedFileLine : BaseEntity
{
    public string Color { get; set; }
    public int Number { get; set; }
    public string Label { get; set; }

    // FK
    public int UploadedFileId { get; set; }
    public UploadedFile UploadedFile { get; set; }
}