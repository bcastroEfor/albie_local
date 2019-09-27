export class LocationAddress {
    id: string = null;
    descripcion: string = null;
    locCiudadId: string = null;
    locationCity: LocationCity = new LocationCity();
}
export class LocationCity {
    id: string = null;
    descripcion: string = null;
    locRegionId: string = null;
    locationRegion: LocationRegion = new LocationRegion();
}
export class LocationRegion {
    id: string = null;
    descripcion: string = null;
    locPaisId: string = null;
    locationCountry: LocationCountry = new LocationCountry();
}
export class LocationCountry {
    id: string;
    descripcion: string = null;
}
