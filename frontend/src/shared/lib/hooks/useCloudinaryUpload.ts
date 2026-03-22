import { useState } from 'react';

interface UploadResult {
  url: string;
  publicId: string;
}

interface UseCloudinaryUpload {
  upload: (file: File) => Promise<UploadResult | null>;
  isUploading: boolean;
  error: string | null;
}

export const useCloudinaryUpload = (): UseCloudinaryUpload => {
  const [isUploading, setIsUploading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const cloudName = import.meta.env.VITE_CLOUDINARY_CLOUD_NAME as string;
  const uploadPreset = import.meta.env.VITE_CLOUDINARY_UPLOAD_PRESET as string;
  const folder = import.meta.env.VITE_CLOUDINARY_FOLDER as string;

  const upload = async (file: File): Promise<UploadResult | null> => {
    setIsUploading(true);
    setError(null);

    const formData = new FormData();
    formData.append('file', file);
    formData.append('upload_preset', uploadPreset);
    formData.append('folder', folder);

    try {
      const response = await fetch(`https://api.cloudinary.com/v1_1/${cloudName}/image/upload`, {
        method: 'POST',
        body: formData,
      });

      if (!response.ok) throw new Error('Upload failed');

      const data = await response.json();
      return { url: data.secure_url as string, publicId: data.public_id as string };
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Upload failed');
      return null;
    } finally {
      setIsUploading(false);
    }
  };

  return { upload, isUploading, error };
};
