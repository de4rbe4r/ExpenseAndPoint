import { React } from 'react';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import '../../App.css';
import ButtonEditUserName from './../User/ButtonEditUserName'; 
import ButtonEditUserPassword from './../User/ButtonEditUserPassword'; 
import CategoryList from './../Category/CategoryList';

const Tools = () => {
    

    return (
        <>
            <Container>
                <Row>
                    <Col sm={12}>
                        <Row>
                            <Col className="d-grid">
                                <ButtonEditUserName />
                            </Col>
                            <Col className="d-grid">
                                <ButtonEditUserPassword />
                            </Col>
                            </Row>
                        <Row>
                            <p></p>
                            <Col className="d-grid">
                                <CategoryList />
                            </Col>
                        </Row>
                    </Col>
                </Row>
            </Container>
        </>
    );
}

export default Tools;
