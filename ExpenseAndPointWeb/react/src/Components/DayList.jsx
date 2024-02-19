import { React, useState, useEffect } from 'react';
import ListGroup from 'react-bootstrap/ListGroup';
import '../App.css';
import useWindowDimensions from '../Hooks/useWindowDimensions.jsx'
import Row from 'react-bootstrap/Row';


const DayList = ({ sendClickedDate}) => {
    // Формирование текущей даты
    var today = new Date();
    var day = today.getDate();
    var month = today.getMonth() + 1;
    if (month < 10) {
        month = '0' + month;
    }
    var year = today.getFullYear();
    var currentDate = year + '-' + month + '-' + day;
    var hours = today.getHours();
    if (hours < 10) {
        hours = '0' + hours;
    }
    var minutes = today.getMinutes();
    if (minutes < 10) {
        minutes = '0' + minutes;
    }

    const { height, width } = useWindowDimensions();

    const [daysArray, setDaysArray] = useState([]);

    useEffect(() => {
        var tempArray = []
        for (var i = 1; i <= day; i++) {
            if (i < 10) tempArray.push(year + '-' + month + '-0' + i);
            else tempArray.push(year + '-' + month + '-' + i);
        }
        setDaysArray(tempArray);
    }, []);



    return (
        <>
            <ListGroup className="overflow-auto"
            style={{
                height: 0.75 * height,
                overflow: scroll
                }}>
                {
                    daysArray.map((d, index) => (
                        <ListGroup.Item action variant="dark" key={d} value={d} onClick={event => sendClickedDate(d) }>
                            <Row className="justify-content-md-center">
                                {d}
                            </Row>
                        </ListGroup.Item>
                    ))
                }
            </ListGroup>
        </>
    );
}

export default DayList;
