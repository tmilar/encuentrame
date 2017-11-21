import {bsasBoundaryPoints} from '../config/locationProperties'
import {Location} from 'expo';
import {Alert} from "react-native";
import PermissionsHelper from '../util/PermissionsHelper';

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

  /**
   * Persistently request Location permission to user.
   *
   * If not accepted, will display an Alert (react-native) error message
   * and try to request again, indefinitely.
   *
   * @returns {Promise.<void>}
   */
  requireLocationPermission = async () => {
    await PermissionsHelper.askPermission("LOCATION", "ubicación", 3000);
  };


  /**
   * Get current device location.
   *
   * @param options: enableHighAccuracy
   * @returns {Promise.<{latitude, longitude, accuracy: (*|number|Number), heading, speed, timestamp}>}
   */
  getDeviceLocation = async (options) => {

    let location = await Location.getCurrentPositionAsync(options);
    let coords = location.coords;

    return {
      latitude: coords.latitude,
      longitude: coords.longitude,
      accuracy: coords.accuracy,
      heading: coords.heading,
      speed: coords.speed,
      timestamp: location.timestamp
    };
  };


  getLocationSurroundings = (location) => {
      return {
        surroundings: [{
          latitude: location.latitude + 0.0075,
          longitude: location.longitude + 0.0075
        },
          {
            latitude: location.latitude + 0.0075,
            longitude: location.longitude - 0.0075

          },
          {
            latitude: location.latitude - 0.0075,
            longitude: location.longitude + 0.0075

          },
          {
            latitude: location.latitude - 0.0075,
            longitude: location.longitude - 0.0075

          }
        ],
        zoomProps: { edgePadding: { top: 100, right: 100, bottom: 100, left: 100}, animated: true}
      };
  };


}

let geolocationService = new GeolocationService();
export default geolocationService;
