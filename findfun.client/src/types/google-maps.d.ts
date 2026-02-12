declare namespace google {
  export namespace maps {
    export class Map {
      constructor(el: HTMLElement, opts?: any)
      setCenter(center: { lat: number; lng: number }): void
    }
    export class Marker {
      constructor(opts?: any)
      addListener(evt: string, cb: () => void): void
      setMap(map: Map | null): void
    }
    export class Size {
      constructor(w: number, h: number)
    }
    export class InfoWindow {
      constructor(opts?: any)
      open(map?: Map, marker?: Marker): void
      setContent(c: string): void
    }
    export const Animation: any
  }
}
