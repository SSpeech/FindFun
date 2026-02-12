export function useProductCard(props: any, emit: any) {
  const scrollToMarker = () => {
    emit('scroll-to-marker', props.item.id)
  }

  return {
    scrollToMarker,
  }
}
