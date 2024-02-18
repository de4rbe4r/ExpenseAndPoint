import { React, useState, useEffect } from 'react';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import { GetExpensesListByUserIdAndDateUrl, GetCategoryByIdUrl } from "../Urls/UrlList";
import Cookies from 'universal-cookie';
import AddCategoryBtn from './AddCategoryBtn'
import ExpenseForm from './ExpenseForm.jsx'
import ExpensesList from './ExpensesList.jsx';
import PieChart from './PieChart.jsx';
import AlertModal from './AlertModal.jsx';
import axios from 'axios';
import Button from 'react-bootstrap/Button';



const Day = () => {
    const cookies = new Cookies();

    const [expensesList, setExpensesList] = useState(null);
    const [categoryList, setCategoryList] = useState([]);
    const [isDataUpdated, setDataUpdated] = useState(false);
    const [showForm, setShowForm] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const [showAlertModal, setShowAlertModal] = useState(false);
    const [titleAlert, setTitleAlert] = useState("");

    var today = new Date();
    var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();

    const [userId, setUserId] = useState(cookies.get('userId'));

    useEffect(() => {
        axios.get(GetCategoryByIdUrl + userId)
            .then(res => {
                setCategoryList(res.data)
            })
            .catch(function (error) {
                if (error.response) {
                    setShowAlertModal(true);
                    setErrorMessage(error.message + ". " + error.response.data.detail);
                    setTitleAlert("Ошибка");
                }
            });
    }, [isDataUpdated]);

    useEffect(() => {
        axios.post(GetExpensesListByUserIdAndDateUrl, getRequest)
            .then(res => {
                setExpensesList(res.data);
            })
            .catch(function (error) {
                if (error.response) {
                    setErrorMessage(error.message + ". " + error.response.data.detail);
                }
            });
    }, [isDataUpdated]);

    var getRequest = {
        date: date,
        userId: userId,
    }

    const setIsShowForm = (data) => {
        setShowForm(data);
    }

    const setIsDataUpdated = (data) => {
        setDataUpdated(data);
    }

    const setIsShowAlertModal = (data) => {
        setShowAlertModal(data);
    }

    const openForm = (event) => {
        event.preventDefault();
        setShowForm(true);
    }

    return (
        <>
            <AlertModal showModal={showAlertModal} setIsShowModal={setIsShowAlertModal} errorMessage={errorMessage} title={titleAlert} />
            <Container>
                <ExpenseForm action="Добавить" setIsDataUpdated={setIsDataUpdated} categoryList={categoryList} expenseToEdit={undefined}
                    isShowForm={showForm} setIsShowForm={setIsShowForm} userId={userId} isDataUpdated={isDataUpdated} />
                <Row>
                    <Col sm={4} >
                        <ExpensesList expensesList={expensesList} categoryList={categoryList} userId={userId} setIsDataUpdatedToParent={setIsDataUpdated}
                            isDataUpdated={isDataUpdated} />
                    </Col>
                    <Col sm={7}>
                        <Row>
                            <Col className="d-grid">
                                <Button variant="outline-light" size="lg" onClick={openForm}>Добавить расход</Button>
                            </Col>
                            <Col className="d-grid">
                                <AddCategoryBtn setIsDataUpdated={setIsDataUpdated} isDataUpdated={isDataUpdated} />
                            </Col>
                        </Row>
                        <Row>
                            <p></p>
                            <PieChart expensesList={expensesList} categoryList={categoryList} />
                        </Row>
                    </Col>
                </Row>
            </Container>
        </>
    );
}

export default Day;