import React from 'react';
import {ActivityIndicator, Text, View} from "react-native";

export default LoadingIndicator = ({size, text}) =>
  <View style={{
    position: 'absolute',
    left: 0,
    right: 0,
    top: 0,
    bottom: 0,
    alignItems: 'center',
    justifyContent: 'center'
  }}>
    <ActivityIndicator size={size || "small"}/>
    { (text && text.length) && <Text>{text}</Text>}
  </View>
