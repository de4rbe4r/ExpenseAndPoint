import { React, useState, useEffect } from 'react';
import ListGroup from 'react-bootstrap/ListGroup';
import '../App.css';
import Row from 'react-bootstrap/Row';


const DayList = ({ sendClickedDate, height, dateStart, dateEnd}) => {
    const [daysArray, setDaysArray] = useState([]);
    useEffect(() => {
        var tempArray = []
        var dateS = new Date(dateStart);
        var dateE = new Date(dateEnd);
        var day, month, year;

        if (dateS.getDate() > dateE.getDate()) {
            var temp = dateE;
            dateE = dateS;
            dateS = temp;
        }

        while (dateS.getDate() !== dateE.getDate()) {
            day = dateS.getDate();
            if (day < 10) {
                day = '0' + day;
            }
            month = dateS.getMonth() + 1;
            if (month < 10) {
                month = '0' + month;
            }
            year = dateS.getFullYear();

            tempArray.push(year + '-' + month + '-' + day);
            dateS.setDate(dateS.getDate() + 1);
        }

        day = dateE.getDate();
        if (day < 10) {
            day = '0' + day;
        }
        month = dateE.getMonth() + 1;
        if (month < 10) {
            month = '0' + month;
        }
        year = dateE.getFullYear();
        tempArray.push(year + '-' + month + '-' + day);

        setDaysArray(tempArray);
    }, [dateStart, dateEnd]);


    return (
        <>
            <ListGroup className="overflow-auto"
            style={{
                height: height,
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
