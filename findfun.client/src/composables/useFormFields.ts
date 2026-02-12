import type { FormField } from '@/config/TabsConfig'

import { fieldRegistry } from '@/config/TabsConfig'

export function useFormFields(type: string): FormField[]
{
  return Object.values(fieldRegistry)
    .filter((field) => field.types.includes(type))
    .map((field): FormField => ({
      model: field.model,
      component: field.component,
      label: field.labelByType?.[type] ?? field.label,
      placeholder: field.placeholderByType?.[type] ?? field.placeholder,
      props: field.props,
      types: field.types,
    }))
}
