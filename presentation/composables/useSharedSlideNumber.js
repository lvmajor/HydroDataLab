import { useStorage } from '@vueuse/core'

// Storage events connect audience/presenter tabs, including previews in the same
// window. Reading storage also restores the state for a late-opening window.
export function useSharedSlideNumber(name, initial, min, max) {
  const normalize = value => {
    const number = Number(value)
    return Number.isInteger(number) && number >= min && number <= max
      ? number
      : initial
  }
  return useStorage(`hydrodatalab:slides:v1:${name}`, initial, undefined, {
    serializer: {
      read: value => normalize(value),
      write: value => String(normalize(value)),
    },
  })
}
