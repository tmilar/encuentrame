import React from "react";
import { Container, Header, Title, Left, Icon, Right, Button, Body, Content,Text, Card, CardItem } from "native-base";

const EncuentrameHeader = props =>
  <Header>
    <Left>
      <Button
        transparent
        onPress={() => alert("hola")}>
        <Icon name="paper-plane" />
        <Text style={{fontSize: 8}}>Nueva actividad</Text>
      </Button>
    </Left>

    <Body>
      <Title></Title>
    </Body>
    <Right>
      <Button
        transparent
        onPress={() => alert("hola")}>
        <Icon name="menu" />
      </Button>
    </Right>
  </Header>
;

export default EncuentrameHeader;
