import React, {Component} from 'react';
import {
  Dimensions,
  ScrollView,
  StyleSheet,
  Text,
  TouchableOpacity,
  View,
} from 'react-native';
import {Svg} from 'expo';
const {Polyline} = Svg;

const WIDTH = Dimensions.get('window').width;
const HEIGHT = 56;
const ARROW_WIDTH = 16;
const MIN_CELL_WIDTH = 100;
const selectedColor = '#FFFD00'; //'#363a45';
const notSelectedColor = '#959400'; //'#252831';

const successColor = "#4eff4d"; // or "#AAFB00" //check: http://paletton.com/#uid=51S0u0kSFT-3Mtj7K8UGjqjlz1W
const skippedColor = "#6A6A6A";

export default class TabProgressTracker extends Component {

  static defaultProps = {
    items: ['¿Cuándo?', '¿Bien o Mal?', '¿Dónde?'/*, 'Boton 3',/* 'Boton 4'*/],
    selectedIndex: 0,
    skippedItems: [],
    resolvedItems: [],
    onItemPress: (index) => console.log(`Prsesed item: #${index} : ${items[index]}`)
  };

  state = {
    selectedIndex: this.props.selectedIndex,
  };

  // _arrowView() returns a view containing a triangle A, or a view containing two small triangles, C and D.
  // For the first left arrow view and the last right arrow view, it returns a rect arrow view.
  //
  // --------
  // |\\  C |
  // | \\   |
  // |  \\  |
  // |   \\ |
  // |A   \\|
  // |   // |
  // |  //  |
  // | //   |
  // |//  D |
  // --------
  _arrowView(index: number,
             totalCount: number,
             isLeft: boolean,
             isSelected: boolean) {
    let color = this.getItemColor(index, isSelected);
    if (isLeft) {
      if (index === 0) {
        // For the first cell, add a rect arrow view
        return (
          <View style={[styles.leftArrowView, {backgroundColor: color}]}/>
        );
      } else {
        // For other cell, add an arrow view containing C, D.
        return (
          <View style={[styles.leftArrowView]}>
            <View style={[styles.triangleC, {borderTopColor: color}]}/>
            <View style={[styles.triangleD, {borderBottomColor: color}]}/>
          </View>
        );
      }
    } else {
      // Right arrow view
      if (index === totalCount - 1) {
        // For the last cell, add a rect arrow view
        return (
          <View style={[styles.rightArrowView, {backgroundColor: color}]}/>
          /*
           <View style={[styles.leftArrowView]}>
           <View style={[styles.triangleC, {borderTopColor: color}]}/>
           <View style={[styles.triangleD, {borderBottomColor: color}]}/>
           </View>
           */
        );
      } else {
        // An arrow view containing A and polyline.
        return (
          <View style={[styles.rightArrowView]}>
            <View
              style={[
                styles.triangleA,
                {
                  borderLeftColor: this.getItemColor(index, isSelected),
                },
              ]}
            />
            <Svg width={ARROW_WIDTH} height={HEIGHT}>
              <Polyline
                fill="none"
                stroke="gray"
                strokeWidth="2"
                points={
                  '0,-2, ' +
                  (ARROW_WIDTH - 1) +
                  ',' +
                  HEIGHT / 2 +
                  ' 0,' +
                  (HEIGHT + 2)
                }
              />
            </Svg>
          </View>
        );
      }
    }
  }

  getItemColor(index, isSelected) {
    if(this.props.skippedItems.includes(index))
      return skippedColor;

    if(this.props.resolvedItems.includes(index))
      return successColor;

    return isSelected ? selectedColor : notSelectedColor;
  }


  _arrowButtons(items: Array<string>, cellWidth: number) {
    let totalCount = items.length;

    return items.map((item, index) => {
      let left = 0;
      let isSelected = index === this.props.selectedIndex;

      if (totalCount === 1 || index === 0) {
        left = 0;
      } else {
        left = (cellWidth - ARROW_WIDTH) * index;
      }
      let positionStyle = {
        position: 'absolute',
        top: 0,
        left,
        height: HEIGHT,
        width: cellWidth,
      };

      return (
        <TouchableOpacity
          key={index}
          style={positionStyle}
          onPress={() => {
            this.props.onItemPress(index);
          }}>
          {// Left arrow view
            this._arrowView(index, totalCount, true, isSelected)
          }
          <View
            style={[
              styles.titleWrapper,
              styles.center,
              styles.bgColor,
              {
                backgroundColor: this.getItemColor(index, isSelected),
              },
            ]}>
            <Text style={styles.titleActionText}>{item}</Text>
          </View>
          {// Right arrow view
            this._arrowView(index, totalCount, false, isSelected)
          }
        </TouchableOpacity>
      );
    });
  }

  calculateCellDimensions(totalCount) {
    let cellWidth = WIDTH;
    let progressBarWidth = WIDTH;

    if (totalCount > 1) {
      cellWidth = ( WIDTH / totalCount ) + ARROW_WIDTH;
      if (cellWidth < MIN_CELL_WIDTH) {
        cellWidth = MIN_CELL_WIDTH;
      }
      progressBarWidth = (cellWidth - ARROW_WIDTH) * totalCount;
    }

    return {cellWidth, progressBarWidth};
  }

  render() {
    let items = this.props.items;
    let {cellWidth, progressBarWidth} = this.calculateCellDimensions(items.length);

    return (
      <View style={{height: HEIGHT}}>
        <ScrollView
          style={styles.progressTracker}
          horizontal={true}
          bounces={false}
          showsHorizontalScrollIndicator={false}>
          <View style={[styles.scrollViewContent, {width: progressBarWidth}]}>
            {this._arrowButtons(items, cellWidth)}
          </View>
        </ScrollView>
      </View>
    );
  }
}

const styles = StyleSheet.create({
  progressTracker: {
    height: HEIGHT,
    width: WIDTH,
  },
  scrollViewContent: {
    height: HEIGHT,
  },
  center: {
    alignItems: 'center',
    justifyContent: 'center',
  },
  bgColor: {
    backgroundColor: '#252831',
  },
  titleWrapper: {
    position: 'absolute',
    top: 0,
    left: ARROW_WIDTH,
    right: ARROW_WIDTH,
    height: HEIGHT,
  },
  titleActionText: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#0F0F05'//'green',
  },

  //Arrow
  leftArrowView: {
    position: 'absolute',
    top: 0,
    height: HEIGHT,
    width: ARROW_WIDTH,
    left: 0,
  },
  rightArrowView: {
    position: 'absolute',
    top: 0,
    right: 0,
    height: HEIGHT,
    width: ARROW_WIDTH,
  },
  triangleA: {
    position: 'absolute',
    top: 0,
    left: 0,
    width: 0,
    height: 0,
    borderTopWidth: HEIGHT / 2,
    borderBottomWidth: HEIGHT / 2,
    borderLeftWidth: ARROW_WIDTH - 2,
    borderTopColor: 'transparent',
    borderBottomColor: 'transparent',
  },
  triangleC: {
    position: 'absolute',
    top: 0,
    right: 0,
    width: 0,
    height: 0,
    borderLeftWidth: ARROW_WIDTH - 2,
    borderRightWidth: 0,
    borderTopWidth: HEIGHT / 2,
    borderLeftColor: 'transparent',
    borderRightColor: 'transparent',
  },
  triangleD: {
    position: 'absolute',
    bottom: 0,
    right: 0,
    width: 0,
    height: 0,
    borderLeftWidth: ARROW_WIDTH - 2,
    borderRightWidth: 0,
    borderBottomWidth: HEIGHT / 2,
    borderLeftColor: 'transparent',
    borderRightColor: 'transparent',
  },
});
