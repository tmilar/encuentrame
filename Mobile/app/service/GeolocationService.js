import {bsasBoundaryPoints} from '../config/locationProperties'

class GeolocationService {

  /**
   * Get region object from an array of points of a square.
   *
   * @param points array [minX, maxX, minY, maxY]; each point :: {{latitude:number, longitude: float}}
   * @returns {{latitude: number, longitude: number, latitudeDelta: number, longitudeDelta: number}}
   */
  regionContainingPoints(points) {
    let minX, maxX, minY, maxY;

    // init first point
    ((point) => {
      minX = point.latitude;
      maxX = point.latitude;
      minY = point.longitude;
      maxY = point.longitude;
    })(points[0]);

    // calculate rect
    points.map((point) => {
      minX = Math.min(minX, point.latitude);
      maxX = Math.max(maxX, point.latitude);
      minY = Math.min(minY, point.longitude);
      maxY = Math.max(maxY, point.longitude);
    });

    let midX = (minX + maxX) / 2;
    let midY = (minY + maxY) / 2;
    let midPoint = [midX, midY];

    let deltaX = (maxX - minX);
    let deltaY = (maxY - minY);

    return {
      latitude: midX,
      longitude: midY,
      latitudeDelta: deltaX,
      longitudeDelta: deltaY,
    };
  }

  /**
   * Get BsAs City region.
   *
   * @returns {{latitude, longitude, latitudeDelta, longitudeDelta}|*}
   */
  getBsAsRegion() {
    return this.regionContainingPoints(bsasBoundaryPoints);
  }
}

let geolocationService = new GeolocationService();
export default geolocationService;
