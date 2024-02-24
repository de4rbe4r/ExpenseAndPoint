import { React, useState, useEffect } from 'react';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import { GetExpensesListByUserIdAndDateUrl, GetCategoryByIdUrl } from "../../Urls/UrlList";
import Cookies from 'universal-cookie';
import ExpenseForm from '../Expense/ExpenseForm.jsx'
import ExpensesList from '../Expense/ExpensesList.jsx';
import PieChart from '../PieChart.jsx';
import AlertModal from '../AlertModal.jsx';
import axios from 'axios';
import Button from 'react-bootstrap/Button';
import useWindowDimensions from '../../Hooks/useWindowDimensions.jsx'
import CategoryForm from '../Category/CategoryForm';
import ExpensesHistoryList from '../Expense/ExpensesHistoryList ';

const Day = () => {
    const cookies = new Cookies();
    const { height, width } = useWindowDimensions();

    const [expensesList, setExpensesList] = useState(null);
    const [categoryList, setCategoryList] = useState([]);
    const [isDataUpdated, setDataUpdated] = useState(false);
    const [showForm, setShowForm] = useState(false);
    const [showCategoryForm, setShowCategoryForm] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const [showAlertModal, setShowAlertModal] = useState(false);
    const [titleAlert, setTitleAlert] = useState("");
    const [userId, setUserId] = useState(cookies.get('userId'));

    var today = new Date();
    var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
    const config = {
        headers: { Authorization: `Bearer ${cookies.get('access_token')}` }
    };

    useEffect(() => {
        axios.get(GetCategoryByIdUrl + userId, config)
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
        axios.post(GetExpensesListByUserIdAndDateUrl, getRequest, config)
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

    const setIsShowCategoryForm = (data) => {
        setShowCategoryForm(data);
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

    const openCategoryForm = (event) => {
        event.preventDefault();
        setShowCategoryForm(true);
    }

    return (
        <>
            <AlertModal showModal={showAlertModal} setIsShowModal={setIsShowAlertModal} errorMessage={errorMessage} title={titleAlert} />
            <Container>
                <ExpenseForm action="Добавить" setIsDataUpdated={setIsDataUpdated} categoryList={categoryList} expenseToEdit={undefined}
                    isShowForm={showForm} setIsShowForm={setIsShowForm} userId={userId} isDataUpdated={isDataUpdated} />
                <CategoryForm action="Добавить" categoryToEdit={undefined}
                    isShowForm={showCategoryForm} setIsShowForm={setIsShowCategoryForm} userId={userId} setIsDataUpdated={setIsDataUpdated} isDataUpdated={isDataUpdated} />
                <Row>
                    <Col sm={4} >
                        <ExpensesList expensesList={expensesList} categoryList={categoryList} userId={userId} setIsDataUpdatedToParent={setIsDataUpdated}
                            isDataUpdated={isDataUpdated} height={0.85 * height} />
                    </Col>
                    <Col sm={7}>
                        <Row>
                            <Col className="d-grid">
                                <Button variant="outline-light" size="lg" onClick={openForm}>Добавить расход</Button>
                            </Col>
                            <Col className="d-grid">
                                <Button variant="outline-light" size="lg" onClick={openCategoryForm}>Добавить категорию</Button>
                            </Col>
                        </Row>
                        <Row>
                            <p></p>
                            <PieChart expensesList={expensesList} categoryList={categoryList} />
                        </Row>
                        <Row>
                            <p></p>
                            <ExpensesHistoryList isDataUpdated={isDataUpdated} height={ 0.35 * height} />
                        </Row>
                    </Col>
                </Row>
            </Container>
        </>
    );
}

export default Day;