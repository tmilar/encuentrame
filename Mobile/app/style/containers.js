import { StyleSheet } from 'react-native';
import { Constants } from 'expo';

const containers = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  },
  statusBar: {
    backgroundColor: "#3DB097",
    paddingTop: Constants.statusBarHeight,
  }
});

export default containers;
