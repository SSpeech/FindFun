export function useImageUpload(form: any)
{
  const imagePreviews = ref<string[]>([])

  function onImageSelect(event: any)
  {
    imagePreviews.value = []
    const files = event.files || []
    if (files.length > 0)
    {
      form.value.image = Array.from(files)
      files.forEach((file: File) =>
      {
        const reader = new FileReader()
        reader.onload = (e) =>
        {
          if (typeof e.target?.result === 'string')
          {
            imagePreviews.value.push(e.target.result)
          }
        }
        reader.readAsDataURL(file)
      })
    }
  }

  function onImageUpload(event: unknown)
  {
    // Optional: implement actual upload logic here
  }

  return { imagePreviews, onImageSelect, onImageUpload }
}
