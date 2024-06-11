namespace ProjectTemplate2024.Contracts.Account.UploadImage;

public record UploadImageRequest(byte[] data, string fileName, long fileSize);
