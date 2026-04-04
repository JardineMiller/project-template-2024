namespace ProjectTemplate2024.Contracts.Account.UploadUserAvatar;

public record UploadUserAvatarRequest(byte[] Data, string FileName, long FileSize);
