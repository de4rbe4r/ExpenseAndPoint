import { React, useState, useEffect } from 'react';
import ListGroup from 'react-bootstrap/ListGroup';
import '../../App.css';
import Row from 'react-bootstrap/Row';
import axios from 'axios';
import { GetExpensesHistoryListByUserId } from '../../Urls/UrlList';
import Cookies from 'universal-cookie';
import AlertModal from '../AlertModal.jsx';

const ExpensesHistoryList = ({ isDataUpdated, height }) => {
    const cookies = new Cookies();
    const config = {
        headers: { Authorization: `Bearer ${cookies.get('access_token')}` }
    };
    const [userId, setUserId] = useState(cookies.get('userId'));
    const [username, setUsername] = useState(cookies.get('userName'));
    const [expensesHistoryList, setExpensesHistoryList] = useState(null);
    const [errorMessage, setErrorMessage] = useState("");
    const [showAlertModal, setShowAlertModal] = useState(false);
    const [titleAlert, setTitleAlert] = useState("");

    useEffect(() => {
        axios.get(GetExpensesHistoryListByUserId + userId, config)
            .then(res => {
                setExpensesHistoryList(res.data)
            })
            .catch(function (error) {
                if (error.response) {
                    setShowAlertModal(true);
                    setErrorMessage(error.message + ". " + error.response.data.detail);
                    setTitleAlert("Ошибка");
                }
            });
    }, [isDataUpdated]);

    const setIsShowAlertModal = (data) => {
        setShowAlertModal(data);
    }

    if (expensesHistoryList === undefined) return <img src="/loading.gif" style={{
        width: "100px",
    }}></img>;

    return (
        <>
            <AlertModal showModal={showAlertModal} setIsShowModal={setIsShowAlertModal} errorMessage={errorMessage} title={titleAlert} />

            <ListGroup className="overflow-auto"
            style={{
                height: height,
                overflow: scroll
                }}>
                <ListGroup.Item variant="dark" key='Title' value='Title'>
                    <Row className="justify-content-md-center">
                        История изменений</Row>
                </ListGroup.Item>
                {(expensesHistoryList !== null ) ?
                    expensesHistoryList.map((e, index) => (
                        <div key={`div-${e.id}`}>
                            <ListGroup.Item action variant="dark" key={e.id} value={e.id}>
                                <Row className="justify-content-md-center" value={e.id}>
                                    {(e.action !== "Изменил") ? (e.dateCreated.replace('T', " ").substr(0, 19) + " " + username + " "
                                        + e.action + " (" + e.newCategoryTitle + " " + e.newDateTime.replace('T', " ").substr(0, 16)
                                        + " " + e.newAmount + "₽)"
                                    ) :
                                        (
                                            e.dateCreated.replace('T', " ").substr(0, 19) + " " + username + " "
                                            + e.action + " (" + e.oldCategoryTitle + " " + e.oldDateTime.replace('T', " ").substr(0, 16)
                                            + " " + e.oldAmount + "₽) на (" + e.newCategoryTitle + " " + e.newDateTime.replace('T', " ").substr(0, 16)
                                            + " " + e.newAmount + "₽)"
                                        )
                                    }
                                    </Row>
                                </ListGroup.Item>
                        </div>
                        )) : null
                }
            </ListGroup>
        </>
    );
}

export default ExpensesHistoryList;
