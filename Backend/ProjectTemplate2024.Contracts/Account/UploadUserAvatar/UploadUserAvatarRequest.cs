namespace ProjectTemplate2024.Contracts.Account.UploadUserAvatar;

public record UploadUserAvatarRequest(byte[] data, string fileName, long fileSize);
